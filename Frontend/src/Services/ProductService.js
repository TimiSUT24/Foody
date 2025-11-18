import {api} from "../Api/api"

export const ProductService = {
    async getProducts(filters = {}) {
        const response = await api.get("/products/filter", {params: filters});
        return response.data;
    }
}