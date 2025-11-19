import api from '../Api/api'

export const CategoryService = {

    async getCategoryTree(){      
            const response = await api.get("/api/Category/tree");
            return response.data;           
    }

    

}