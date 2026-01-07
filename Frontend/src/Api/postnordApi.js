import axios from "axios";

export const postnordApi = axios.create({
  baseURL: "http://localhost:3000",
  withCredentials: false, // IMPORTANT
});