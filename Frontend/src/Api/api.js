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

const shouldRetry = (config) => {
    const url = config.url?.toLowerCase();
    return !url?.includes("/auth/login") && !url?.includes("/auth/register");
};

//response interceptor
api.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;
        if(error.response?.status === 401 && !originalRequest._retry && shouldRetry(originalRequest)){
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

    const data = error.response?.data;
    let messages = [];
    if(data?.error){
        messages.push(data.error);
    }else if(data?.errors){
        messages = Object.values(data.errors).flat()
    }else if(data?.message){
        messages.push(data.message);
    }else{
        messages.push("An unkown error occured");
    }

    error.messages = messages;

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