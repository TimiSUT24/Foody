import { stripeApi } from "../Api/stripeApi";

export const createPaymentIntent = async (cartItems,shipping,shippingTax) => {
  const response = await stripeApi.post("/create-payment-intent", {
    cartItems,
    shipping,
    shippingTax
  });

  return response.data;
};
