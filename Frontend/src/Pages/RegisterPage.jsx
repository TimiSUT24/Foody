import {useState} from 'react'
import { useNavigate } from 'react-router-dom';
import { AuthService } from '../Services/AuthService'
import "../CSS/RegisterPage.css"

export default function RegisterPage(){
    const [formData, setFormData] = useState({
        userName: "",
        firstName: "",
        lastName: "",
        phonenumber: "",
        email: "",
        password: ""
    });
    const [message, setMessage] = useState("");
    const {register} = AuthService;
    const navigate = useNavigate();
    const [error, setError] = useState([]);

    const handleChange = (e) => {
        setFormData({...formData, [e.target.name]: e.target.value})
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        if(!formData.email || !formData.userName || !formData.password){
            setError(["Empty fields"]);
            return;
        }

        try{
            const body = {
                userName: formData.userName,
                firstName: formData.firstName,
                lastName: formData.lastName,
                phoneNumber: formData.phonenumber,
                email: formData.email,
                password: formData.password
            }
                const response = await register(body)
                if(response.status === 201){
                    setError([])
                    setMessage("Registration successful");
                    setTimeout(() => {
                    navigate("/login")
                    },3000)
                    
                }
        }catch(err){
            setError(err.messages || ["Something went wrong"]);
            
        }

    }


    return(
        <div className="register-page">
            <h1>Skapa ditt konto</h1>
            <form onSubmit={handleSubmit}>

                <div className="register-row">
                    <div className="register-row-input">
                        <h3 >Förnamn</h3>
                        <input 
                            type="text"
                            name="firstName"
                            placeholder="Jan"
                            value={formData.firstName}
                            onChange={handleChange}/>
                    </div>
               
                    <div  className="register-row-input">
                        <h3 >Efternamn</h3>
                        <input 
                            type="text"
                            name="lastName"
                            placeholder="Andersson"
                            value={formData.lastName} 
                            onChange={handleChange}/>
                    </div>
                    
                </div>
                
                <div className="register-column">
                    <h3>Användarnamn</h3>
                    <input 
                    type="text"
                    name="userName"
                    placeholder="Andersson123"
                    value={formData.userName}
                    onChange={handleChange}/>
            
                    <h3>Telefonnummer</h3>
                    <input 
                    type="tel"
                    name="phonenumber"
                    placeholder="0721881423"
                    value={formData.phonenumber} 
                    onChange={handleChange}/>
            
                    <h3>E-post</h3>
                    <input 
                    type="text"
                    name="email"
                    placeholder="exempel@gmail.com"
                    value={formData.email} 
                    onChange={handleChange}/>
                    
                    <h3>Lösenord</h3>
                    <input 
                    type="password"
                    name="password"
                    placeholder="••••••••"
                    value={formData.password} 
                    onChange={handleChange}/>
                </div>
               
                <button type="submit" className="register-button">Registrera</button>
                {error.map((err, i) => (<p key={i}>{err}</p>))}
                {message && <p>{message}</p>}
            </form>

        </div>
    )
}