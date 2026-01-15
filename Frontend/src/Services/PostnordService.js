import api from "../Api/api";

export const PostNordService = {
        getDeliveryOptions: async (recipient) => {
        const response = await api.post("/api/Postnord/options", {
            recipient
        })
        return response.data;
    },

    postalCodeValidation: async (postCode) => {
        const response = await api.post("/api/Postnord/postalCode/Validation", {postCode})
        return response.data;
    }
}

