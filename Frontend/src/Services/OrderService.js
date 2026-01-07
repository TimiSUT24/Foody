import api from "../Api/api";

export const OrderService = {

    myOrders: async () => {
        const response = await api.get("/api/Order/MyOrders");
        return response.data;
    },

    myOrder: async (id) => {
        const response = await api.get(`/api/Order/MyOrder/${id}`);
        return response.data;
    },

    updateOrder: async () => {
        const response = await api.patch("/api/Order/update-status");
        return response.data;
    }

}