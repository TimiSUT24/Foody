import dotenv from "dotenv";
import express from "express";
import cors from "cors";
import fetch from "node-fetch";

dotenv.config();
const app = express();

app.use(cors({
  origin: "https://localhost:5173",
  credentials: true,
}));
app.use(express.json());

const postnord = process.env.POSTNORD_API_KEY;

app.post("/api/shipping/postnord/options", async (req, res) => {
  try {
    const { recipient } = req.body;

    if (!recipient) {
      return res.status(400).json({ error: "recipient is required" });
    }

    const response = await fetch(
      `https://atapi2.postnord.com/rest/shipment/v1/deliveryoptions/bywarehouse?apikey=${postnord}`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Accept-Language": "sv"
        },
        body: JSON.stringify({
          warehouses: [
            {
              id: "Falkenberg",
              address: {
                postCode: "31175", // my warehouse
                street: "Sandgatan 34",
                city: "Falkenberg",
                countryCode: "SE"
              },
              orderHandling: {
                daysUntilOrderIsReady: "0-2"
            }
            }
          ],
           customer: {
        customerKey: "example_request_customer_key"
    },
          recipient: {
            address:{
            postCode: recipient.postCode,
            countryCode: "SE"
            }
          },
        })
      }
    );

    const data = await response.json();

    // Filter home delivery only
    const homeDeliveryOptions = data.warehouseToDeliveryOptions
  .map(wh => {
    return {
      warehouse: wh.warehouse,
      deliveryOptions: wh.deliveryOptions.filter(o => o.type === "home")
    };
  })
  // Remove warehouses with no home delivery options
  .filter(wh => wh.deliveryOptions.length > 0);

res.json(homeDeliveryOptions);
  } catch (err) {
    console.error("PostNord delivery options error:", err);
    res.status(500).json({ error: "PostNord request failed" });
  }
});


app.post("/api/shipping/booking", async (req,res) => {
    try {
    const { shipping, orderId, totalWeight } = req.body;
    const stripeShipping = shipping.shipping
    const address = stripeShipping.address

    if (!shipping) return res.status(400).json({ error: "Shipping info required" });

    // Construct PostNord booking payload
    const payload = {
      messageDate: new Date().toISOString(),
      messageFunction: "Instruction",
      messageId: `order-${orderId}`.substring(0,30),
      application: {
        applicationId: 9999, // sandbox/test ID
        name: "Foody",
        version: "1.0"
      },
      updateIndicator: "Original",
      shipment: [
        {
          shipmentIdentification: {
            shipmentId: orderId.substring(0,30) // can use your internal orderId as shipmentId
          },
          dateAndTimes: {
            loadingDate: new Date().toISOString()
          },
          service: {
            basicServiceCode: shipping.serviceCode, // from selectedOption
            additionalServiceCode: []
          },
          freeText: [],
          numberOfPackages: {
            value: 1
          },
          totalGrossWeight: {
            value: totalWeight, // e.g., sum of cart item weights
            unit: "KGM"
          },
          parties: {
            consignor: {
              issuerCode: "Z12",
              partyIdentification: {
                partyId: "1111111111", // your sandbox party ID
                partyIdType: "160"
              },
              party: {
                nameIdentification: {
                  name: "Foody"
                },
                address: {
                  streets: ["Sandgatan 34"],
                  postalCode: "31175",
                  city: "Falkenberg",
                  countryCode: "SE"
                }
              }
            },
            consignee: {
              party: {
                nameIdentification: { name: shipping.firstName + " " + shipping.lastName },
                address: {
                  streets: [address.line1],
                  postalCode: address.postal_code,
                  city: address.city,
                  countryCode: "SE"
                },
                contact: {
                  contactName: shipping.firstName + " " + shipping.lastName,
                  emailAddress: shipping.email,
                  smsNo: shipping.phoneNumber
                }
              }
            }
          },
          goodsItem: [
            {
              packageTypeCode: "PC",
              items: [
                {
                  itemIdentification: {
                    itemId: "0",
                    itemIdType: "SSCC"
                  },
                  grossWeight: {
                    value: totalWeight,
                    unit: "KGM"
                  }
                }
              ]
            }
          ]
        }
      ]
    };

    const response = await fetch(`https://atapi2.postnord.com/rest/shipment/v3/edi?apikey=${postnord}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(payload)
    });

    const data = await response.json();

    // Return bookingId, tracking URL, etc.
    res.json(data);

  } catch (err) {
    console.error("PostNord booking error:", err);
    res.status(500).json({ error: "PostNord booking failed" });
  }
})


app.listen(3000, () => console.log("Stripe ECE server running on http://localhost:3000"));