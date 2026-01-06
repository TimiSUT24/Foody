import api from "../Api/api";

export const getDeliveryOptions = async (recipient) => {
    const response = await api.post("/api/Postnord/options", {
        recipient
    })
    return response.data;
}