import { chromium } from "playwright";
import fs from "fs-extra";

const urlsFile = "icaUrl.json";
const outputFile = "icaDetails.json";

(async () => {
  const urls = JSON.parse(await fs.readFile(urlsFile, "utf-8"));
  const existingData = (await fs.pathExists(outputFile))
    ? JSON.parse(await fs.readFile(outputFile, "utf-8"))
    : [];

  const browser = await chromium.launch({ headless: false }); // show browser
  const page = await browser.newPage({ locale: "sv-SE" });

  console.log(`Starting to scrape ${urls.length} products...\n`);

  try {
    // Accept cookies if found
    await page.goto(urls[0], { waitUntil: "networkidle" });
    const cookieButton = page.getByRole("button", { name: /Godkänn kakor/i });
    if (cookieButton) {
      await cookieButton.click();
      console.log("Cookie banner accepted.");
    }
  } catch (err) {
    console.log("No cookie banner found.");
  }

  for (let i = 0; i < urls.length; i++) {
    const url = urls[i];
    console.log(`(${i + 1}/${urls.length}) Scraping: ${url}`);

    try {
      await page.goto(url, { waitUntil: "networkidle" });
      await page.waitForSelector("div._grid_vea1m_1 ", { timeout: 2000 });
    } catch (err) {
      console.error(`Failed to scrape url`);
      continue;
    }

    try {
      const product = await page.$eval("div._grid_vea1m_1", (container) => {
        const cleanText = (text) =>
          text.replace(/\s+/g, " ").replace(/~~/g, "").trim();

        const name = container.querySelector("h1")?.innerText.trim() || null;

        const sizeText =
          cleanText(container.querySelector("span.salt-vc")?.innerText || "") ||
          "";

        let weightText = "";
        let comparePrice = "";
        let caPart = "";
        let weightValue = 0;
        let weightUnit = "";

        // Match optional "ca", numeric value, and unit (g or kg)
        const weightMatch = sizeText.match(
          /(ca)?\s*(\d+(?:[.,]\d+)?)\s*(kg|g|l|L)/i
        );

        if (weightMatch) {
          caPart = weightMatch[1] ? weightMatch[1].trim() : ""; // "ca" or ""
          weightValue = parseFloat(weightMatch[2].replace(",", ".")); // numeric value
          weightUnit = weightMatch[3].toLowerCase().trim(); // "g" or "kg"

          // Build readable combined string if needed
          weightText =
            `${caPart ? caPart + " " : ""}${weightValue} ${weightUnit}`.trim();

          // Remove matched portion from the original text to get compare price
          comparePrice = sizeText
            .replace(weightMatch[0], "")
            .replace(/[ ,]+$/, "")
            .trim();
        } else {
          comparePrice = sizeText.trim();
        }

        const priceContainer = container.querySelector(
          'div[class*="sc-gEkIjz cvslSt"]'
        );

        let currentPrice = 0;

        if (priceContainer) {
          const currentSpan = priceContainer.querySelector(
            "span._display_xy0eg_1"
          );

          const parsePrice = (text) => {
            if (!text) return 0;
            // remove non-numeric characters except comma and dot, replace comma with dot
            const num = text.replace(/[^\d,.-]/g, "").replace(",", ".");
            return parseFloat(num) || 0;
          };

          currentPrice = currentSpan ? parsePrice(currentSpan.innerText) : 0;
        }

        const attributes = Array.from(
          container.querySelectorAll('div[class*="salt-p--1"] span')
        )
          .map((span) => span.innerText.trim())
          .filter(Boolean);

        const imgUrl = container.querySelector("img").src;

        //container have same div class helper function to avoid repetition
        const getSectionText = (container, heading) => {
          const box = Array.from(
            container.querySelectorAll("div._box_1qlpx_1")
          ).find(
            (div) =>
              div.querySelector("h2")?.innerText.trim().toLowerCase() ===
              heading.toLowerCase()
          );

          if (!box) return "";

          const text = Array.from(box.querySelectorAll("div.sc-jOnpCo.hKgyxe"))
            .map((inner) => inner.innerText.replace(/\n+/g, " ").trim())
            .filter(Boolean)
            .join("\n")
            .trim();

          return text || "";
        };

        //const productInformation = getSectionText(container, "produktinformation");
        const countryText = getSectionText(container, "ursprungsland");
        const companyText =
          getSectionText(container, "varumärke").replace(/,/g, "").trim() || "";
        const ingrediensText = getSectionText(container, "ingredienser");

        let storage = "";
        let usage = "";
        let allergens = "";
        let productInformation = "";

        const productInfoBox = Array.from(
          container.querySelectorAll("div._box_1qlpx_1")
        ).find(
          (div) =>
            div.querySelector("h2")?.innerText.trim().toLowerCase() ===
            "produktinformation"
        );

        if (productInfoBox) {
          const nodes = Array.from(
            productInfoBox.querySelectorAll("div.sc-jOnpCo.hKgyxe")
          )[0].childNodes;

          let currentSection = "productInformation"; // start collecting raw text
          let collectedText = {
            productInformation: "",
            usage: "",
            allergens: "",
            storage: "",
          };

          nodes.forEach((node) => {
            if (node.tagName === "B") {
              const label = node.innerText.trim().toLowerCase();
              if (label.includes("använd")) currentSection = "usage";
              else if (label.includes("allerg")) currentSection = "allergens";
              else if (label.includes("förvar")) currentSection = "storage";
            } else {
              let text = "";
              if (node.nodeType === 3) text = node.textContent.trim();
              if (node.nodeType === 1) {
                if (node.tagName === "BR") text = "\n";
                else text = node.innerText.trim();
              }
              if (text) collectedText[currentSection] += text + " ";
            }
          });

          // clean up
          productInformation = collectedText.productInformation.trim();
          usage = collectedText.usage.trim();
          allergens = collectedText.allergens.trim();
          storage = collectedText.storage.trim();
        }

        const storageText = getSectionText(container, "förvaring");
        if (storageText) storage = storageText;

        //Nutrition
        const nutritionBox = Array.from(
          container.querySelectorAll("div._box_1qlpx_1")
        ).find(
          (box) =>
            box.querySelector("h2")?.innerText.trim().toLowerCase() ===
            "näringsdeklaration"
        );

        let nutrition = [];
        let nutritionUnitText = "";
        if (nutritionBox) {
          const bTag = Array.from(nutritionBox.querySelectorAll("b")).find(
            (b) => /Näringsvärde per/i.test(b.innerText)
          );
          if (bTag) nutritionUnitText = bTag.innerText.trim();

          const rows = nutritionBox.querySelectorAll(
            "div.sc-jOnpCo.hKgyxe table tbody tr"
          );

          if (rows.length > 1) {
            // First row headers
            const headers = Array.from(rows[0].querySelectorAll("th")).map(
              (th) => th.innerText.trim()
            );

            // Following rows td values
            const dataRows = Array.from(rows)
              .slice(1)
              .map((row) =>
                Array.from(row.querySelectorAll("td")).map((td) =>
                  td.innerText.trim()
                )
              );

            // Build an array of objects (each <tr> object with header/value pairs)
            nutrition = dataRows.map((values) => {
              const obj = {};
              headers.forEach((key, i) => {
                if (values[i]) obj[key] = values[i];
              });
              return obj;
            });
          }
        }

        //Categories
        const breadcrumbContainer = container.querySelector(
          "._grid-item-12_tilop_45"
        );

        let categoryItems = [];

        if (breadcrumbContainer) {
          categoryItems = Array.from(breadcrumbContainer.querySelectorAll("a"))
            .map((a) => a.innerText.trim())
            .filter(Boolean)
            .filter((x) => x.toLowerCase() !== "start");
        }

        const categories = {
          mainCategory: categoryItems[0] || "",
          subCategories: [],
        };

        if (categoryItems.length > 1) {
          categories.subCategories.push({
            name: categoryItems[1] || "",
            subSubCategories: categoryItems.slice(2),
          });
        }

        return {
          name,
          weightText,
          caPart,
          weightValue,
          weightUnit,
          comparePrice,
          currentPrice,
          attributes,
          imgUrl,
          productInformation,
          countryText,
          companyText,
          ingrediensText,
          storage,
          allergens,
          usage,
          nutrition,
          nutritionUnitText,
          categories,
        };
      });

      existingData.push(product);
      await fs.writeFile(
        outputFile,
        JSON.stringify(existingData, "", 2),
        "utf-8"
      );
    } catch (err) {
      console.error(
        `Failed to extract product data for ${url}: ${err.message}`
      );
      continue; // skip this URL
    }
  }
  await browser.close();
})();
