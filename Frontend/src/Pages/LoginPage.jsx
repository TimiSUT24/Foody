import {useState} from 'react'
import {useAuth} from "../Context/AuthContext"
import { useNavigate } from 'react-router-dom'

export default function LoginPage () {

    const {login} = useAuth();
    const navigate = useNavigate();

    const [loginRequest, setLoginRequest] = useState({
        email: "",
        password: ""
    })

    const [error, setError] = useState("");

    const handleChange = (e) => {
        setLoginRequest({ ...loginRequest, [e.target.name]: e.target.value})
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");
    
        try{
            await login(loginRequest.email,loginRequest.password);
            navigate("/");
        }catch(error){
            setError("Fel inloggninguppgifter")
        }
    }


    return (
        <div className="login-page">Login

            <form onSubmit={handleSubmit}>
            <input 
            type="text"
            name="email"
            placeholder="email"
            value={loginRequest.email} 
            onChange={handleChange}/>

            <input 
            type="text"
            name="password"
            placeholder="password"
            value={loginRequest.password} 
            onChange={handleChange}/>

            <button type="submit" className="login-button">Login</button>
            </form>

        </div>
    )
}