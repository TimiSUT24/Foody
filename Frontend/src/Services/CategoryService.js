import api from '../Api/api'

export const CategoryService = {

    async getCategoryTree(){      
            const response = await api.get("/api/Category/tree");
            return response.data;           
    },

    async getCategoryDetails(id){
        const response = await api.get(`/api/Category/${id}`);
        return response.data;
    },

    async getSubCategoryDetails(id){
        const response = await api.get(`/api/Category/subCategory/${id}`);
        return response.data;
    },
    
    async getSubSubCategoryDetails(id){
        const response = await api.get(`/api/Category/subSubCategory/${id}`);
        return response.data;
    }
}