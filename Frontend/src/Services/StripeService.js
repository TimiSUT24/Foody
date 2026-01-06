import api from "../Api/api";

export const createPaymentIntent = async (cartItems,shipping,shippingTax,userId) => {
  const response = await api.post("/api/Stripe/create-payment-intent", {
    cartItems,
    shipping,
    shippingTax,
    userId
  });

  return response.data;
};
