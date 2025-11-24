import {useState} from 'react'
import {useAuth} from "../Context/AuthContext"
import { useNavigate } from 'react-router-dom'
import "../CSS/LoginPage.css"

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

          if (!loginRequest.email || !loginRequest.password) {
            setError("Email and password required");
            return;
            }
   
        try{
            await login(loginRequest.email,loginRequest.password);
            navigate("/");
        }catch(error){
            setError("Fel inloggninguppgifter")
        }
    }


    return (
        <div className="login-page">
            <h1>Logga in</h1>
            <p>Skriv in dina uppgifter här</p>

            <form onSubmit={handleSubmit}>
            <h3>E-post</h3>
            <input
            type="text"
            name="email"
            placeholder="namn@example.com"
            value={loginRequest.email} 
            onChange={handleChange}/>

            <h3>Lösenord</h3>
            <input 
            type="password"
            name="password"
            placeholder="••••••••"
            value={loginRequest.password} 
            onChange={handleChange}/>

            <button type="submit" className="login-button">Logga in</button>
            {error && <p>{error}</p>}
            </form>

        </div>
    )
}