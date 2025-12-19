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
        const response = await api.put("/api/Auth/refreshToken", body);
        return response.data;
    },

      logout: async () => {
        const response = await api.post("/api/Auth/logout");
        return response;
    },

    changePassword: async (body) => {
      const response = await api.put("/api/Auth/change-password", body);
      return response.data;
    },
    
    updateProfile: async (body) => {
      const response = await api.patch("/api/Auth/update-profile", body);
      return response.data;
    }


}