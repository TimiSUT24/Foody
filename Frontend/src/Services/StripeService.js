import { stripeApi } from "../Api/stripeApi";

export const createPaymentIntent = async (cartItems) => {
  const response = await stripeApi.post("/create-payment-intent", {
    cartItems
  });

  return response.data;
};
