import { stripeApi } from "../Api/stripeApi";

export const createPaymentIntent = async (cartItems,shipping,shippingTax,userId) => {
  const response = await stripeApi.post("/create-payment-intent", {
    cartItems,
    shipping,
    shippingTax,
    userId
  });

  return response.data;
};
