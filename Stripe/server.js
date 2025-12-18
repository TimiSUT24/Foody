import express from "express";
import cors from "cors";
import Stripe from "stripe";
import dotenv from "dotenv";

dotenv.config();
const app = express();

app.use(cors({
  origin: "http://localhost:5173",
  credentials: true,
}));
app.use(express.json());

const stripe = new Stripe(process.env.STRIPE_SECRET_KEY);

//Create Payment
app.post("/create-payment-intent", async (req, res) => { //add shipping tax to order 
  try {
    const { cartItems,shipping,shippingTax} = req.body; // frontend must send { cart: [...] }

    if (!cartItems || !cartItems.length) return res.status(400).json({ error: "Cart is empty" });

    const calculateAmount = cartItems.reduce((sum, item) => sum + item.price * 100 * item.qty, 0);
    let calcShippingTax = Math.round(shippingTax * 100)
    let subTotal = calculateAmount + calcShippingTax
    let calculateMoms = subTotal * 1.12 
    const amount = calculateMoms.toFixed();

    const paymentIntent = await stripe.paymentIntents.create({
      amount,
      currency: "sek",
      payment_method_types: [
        "card",
        "klarna"
      ],
      capture_method: "manual",
      shipping: {
        name: shipping.firstName,
        phone: shipping.phoneNumber,
        address: {
          line1: shipping.adress,
          city: shipping.city,
          country: "Sweden",
          state: shipping.state,
          postal_code: shipping.postalCode
        }
      },
      metadata:{
        email:shipping.email,
        lastname: shipping.lastName,
        deliveryOptionId: shipping.deliveryOptionId,
        serviceCode: shipping.serviceCode,
        shippingTax: shippingTax
      }
    });

    res.status(200).json({ clientSecret: paymentIntent.client_secret});
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: err.message });
  }
});

//Capture Payment
app.post("/capture-payment-intent", async (req, res) => {
  try {
    const { paymentIntentId } = req.body;

    const intent = await stripe.paymentIntents.capture(paymentIntentId);

    res.status(200).json(intent);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

//Cancel Payment
app.post("/cancel-payment-intent", async (req, res) => {
  try {
    const { paymentIntentId } = req.body;

    const canceled = await stripe.paymentIntents.cancel(paymentIntentId);

    res.status(200).json(canceled);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

app.listen(3001, () => console.log("Stripe ECE server running on http://localhost:3001"));
