import api from "../Api/api"

export const ProductService = {
    async getProducts(filters = {}) {
        const response = await api.get("/api/Product/filter", {params: filters});
        return response.data;
    }
}