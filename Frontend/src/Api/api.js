import axios from 'axios'

const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL,
    withCredentials: true,
});

//request interceptor
api.interceptors.request.use(
    (config) => {
        return config
    },
    (error) => {
        console.error("Request error:", error);
        return Promise.reject(error);
    }
)

api.interceptors.response.use(
    (response) => response,
    (error) => {
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