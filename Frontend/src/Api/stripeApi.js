import axios from "axios";

export const stripeApi = axios.create({
  baseURL: "http://localhost:3001",
  withCredentials: false, // IMPORTANT
});