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

app.post("/create-payment-intent", async (req, res) => {
  try {
    const { cartItems } = req.body; // frontend must send { cart: [...] }

    if (!cartItems || !cartItems.length) return res.status(400).json({ error: "Cart is empty" });

    const amount = cartItems.reduce((sum, item) => sum + item.price * 100 * item.qty, 0);

    const paymentIntent = await stripe.paymentIntents.create({
      amount,
      currency: "sek",
      automatic_payment_methods: [{ enabled: true }],
    });

    res.status(200).json({ clientSecret: paymentIntent.client_secret });
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: err.message });
  }
});

app.listen(3001, () => console.log("Stripe ECE server running on http://localhost:3001"));
