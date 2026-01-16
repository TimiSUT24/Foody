import { useState } from "react"
import { useAuth } from "../Context/AuthContext";
import "../CSS/User/ProfileSettings.css"

    
export default function ProfileSettings(){
    const {updateProfile,changePassword} = useAuth();
    const [personal, setPersonal] = useState({
        firstName:"",
        lastName:"",
        email:"",
        phoneNumber:""
    })

    const [password, setPassword] = useState({
        currentPass:"",
        newPass:""
    })
     const handleChangePassword = (e) => {
        setPassword({...password, [e.target.name]: e.target.value})
    }

    const handleChange = (e) => {
        setPersonal({ ...personal, [e.target.name]: e.target.value})
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        const body = {
            firstName: personal.firstName,
            lastName: personal.lastName,
            email: personal.email,
            phoneNumber: personal.phoneNumber
        }

        await updateProfile(body)
    }

    const handlePasswordSubmit = async (e) => {
        e.preventDefault();

        const body = {
            currentPassword: password.currentPass,
            newPassword: password.newPass
        }

        await changePassword(body);
    }

    return(
        <div className="profile-setting-container">

            <div className="personal-info">
                <h2 style={{textAlign:"left",marginLeft:20,marginBottom:0}}>Personlig information</h2>
                <p style={{textAlign:"left", marginLeft:20}}>Uppdatera dina personuppgifter och kontaktinformation</p>
                <form className="personal-form" onSubmit={handleSubmit} >
                    <div className="personal-names">

                        <div className="personal-names-div" style={{display:"flex",flexDirection:"column", gap:5}}>
                            <p style={{textAlign:"left", margin:0}}>Förnamn</p>
                            <input className="personal-names-input" type="text" name="firstName" value={personal.firstName} onChange={handleChange} placeholder="Förnamn" style={{height:40,borderRadius:10,border:"1px solid gray"}}/>
                        </div>
                        <div className="personal-names-div" style={{display:"flex",flexDirection:"column", gap:5}}>
                            <p style={{textAlign:"left",margin:0}}>Efternamn</p>
                            <input className="personal-names-input" type="text" name="lastName" value={personal.lastName} onChange={handleChange} placeholder="Efternamn" style={{height:40,borderRadius:10,border:"1px solid gray"}}/>
                        </div>
                    
                    </div>

                    <div style={{display:"flex", flexDirection:"column",gap:5}}>
                        <p style={{textAlign:"left",margin:0}}>E-post</p>
                        <input  className="personal-contact-input" type="email" name="email" value={personal.email} onChange={handleChange} placeholder="Example.com" style={{height:40,borderRadius:10,border:"1px solid gray"}}/>
                    </div>

                    <div  style={{display:"flex", flexDirection:"column",gap:5}}>
                    <p style={{textAlign:"left",margin:0}}>Telefonnummer</p>
                    <input  className="personal-contact-input" type="number" name="phoneNumber" value={personal.phoneNumber} onChange={handleChange} placeholder="0721223344" style={{height:40,borderRadius:10,border:"1px solid gray"}}/>
                    </div>

                    <button className="personal-save-btn">Spara ändringar</button>

                </form>

            </div>

            <div className="change-password">
                <h2 style={{textAlign:"left",marginLeft:20,marginBottom:0}}>Ändra lösenord</h2>
                <p style={{textAlign:"left", marginLeft:20}}>Uppdatera ditt lösenord för att hålla ditt konto säkert</p>
                <form className="password-form" onSubmit={handlePasswordSubmit}>
                    <div  style={{display:"flex", flexDirection:"column",gap:5}}>
                        <p style={{textAlign:"left",margin:0}}>Nurvarande lösenord</p>
                        <input className="password-input" type="password" name="currentPass" value={password.currentPass} onChange={handleChangePassword} style={{height:40,borderRadius:10,border:"1px solid gray"}}/>
                    </div>
                    
                    <div style={{display:"flex", flexDirection:"column",gap:5}}>
                        <p style={{textAlign:"left",margin:0}}>Nytt lösenord</p>
                        <input className="password-input" type="password" name="newPass" value={password.newPass} onChange={handleChangePassword} style={{height:40,borderRadius:10,border:"1px solid gray"}}/>
                    </div>            

                    <button className="change-pass">Ändra lösenord</button>
                </form>

            </div>
            
        </div>
    )
}