import { stripeApi } from "../Api/stripeApi";

export const createPaymentIntent = async (cartItems,shipping) => {
  const response = await stripeApi.post("/create-payment-intent", {
    cartItems,
    shipping
  });

  return response.data;
};
