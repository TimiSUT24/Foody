import { postnordApi } from "../Api/postnordApi";

export const getDeliveryOptions = async (recipient) => {
    const response = await postnordApi.post("/api/shipping/postnord/options", {
        recipient
    })
    return response.data;
}