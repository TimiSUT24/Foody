import api from "../Api/api"

export const AuthService = {

    login: async (email, password) => {
        const response = await api.post("/api/Auth/login", {email, password});
        return response.data;
    },

    register: async (body) => {
        const response = await api.post("/api/Auth/register", body);
        return response;
    },

      refresh: async (body) => {
        const response = await api.post("", {body});
        return response;
    },

      logout: async (body) => {
        const response = await api.post("", {body});
        return response;
    }


}