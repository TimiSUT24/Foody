import api from "../Api/api"

export const ProductService = {
    async getProducts(filters = {}) {
        const response = await api.get("/api/Product/filter", {params: filters});
        return response.data;
    },

    getProductDetails: async (id) => {
        if(!id) throw new Error("Product ID is required");
        const response = await api.get(`/api/Product/details/${id}`)
        return response.data;
    },

    getProductBrands: async (params = {}) => {
        const response = await api.get(`/api/Product/brands`, {params})
        return response.data;
    }
    
};

