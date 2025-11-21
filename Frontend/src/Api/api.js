import axios from 'axios'
import { AuthService } from '../Services/AuthService';

const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL,
    withCredentials: true,
});

//request interceptor
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("accessToken");
        if(token){
            config.headers.Authorization = `Bearer ${token}`
        }
        return config
    },
    (error) => {
        console.error("Request error:", error);
        return Promise.reject(error);
    }
)

//response interceptor
api.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;
        if(error.response?.status === 401 && !originalRequest._retry){
            originalRequest._retry = true;
        

        try{
            const data = await AuthService.refresh();
            localStorage.setItem("accessToken", data.accessToken);

            api.defaults.headers.Authorization = `Bearer ${data.accessToken}`;
            return api(originalRequest);

        }catch(refErr){
            return Promise.reject(refErr);
        }
    }

        if(error.response){
            console.error("API response error:", error.response.status, error.response.data)
        } else if(error.request){
            console.error("No response recieved:",error.request)
        }else{
            console.error("Axios error:", error.message)
        }
        return Promise.reject(error);
    }
    
);

export default api;