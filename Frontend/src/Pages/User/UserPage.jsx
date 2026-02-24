import { Link } from "react-router-dom";
import { useAuth } from "../../Context/AuthContext";
import { PiPackage } from "react-icons/pi";
import { DiAptana } from "react-icons/di";
import "../../CSS/User/UserPage.css";


export default function UserPage(){
        const {user} = useAuth();
    return (
        <div className="user-page">

            <div id="user-header">
                <h1>Välkommen {user.username}</h1>
                <p>Här kan du hantera dina beställningar och konto uppgifter.</p>

            </div>

            <div id="user-content">

                <div className="user-options">
                    <Link to={"/user-profile-page"}
                 state={{tab: "orders"}}
                 style={{textDecoration:"none"}}>
                <button id="user-order">
                    <div className="order-image">
                        <icon id="shipping-img" style={{width:30,height:30}}><PiPackage style={{width:60,height:60,color:"hsl(28, 80%, 40%)"}}/></icon>
                    </div>
                    <h2 className="user-order-h2"style={{marginBottom:"0"}}>Mina beställningar</h2>
                    <p className="user-order-p">Se alla dina beställningar och följ leveranstatus i realtid.</p>
                </button>
                </Link>

                <Link to={"/user-profile-page"}
                state={{tab:"settings"}}
                style={{textDecoration:"none"}}>
                <button id="user-settings">
                    <div className="order-image">
                        <icon id="shipping-img" style={{width:30,height:30}}><DiAptana style={{width:60,height:60,color:"hsl(28, 80%, 40%)"}}/></icon>
                    </div>
                    <h2 className="user-settings-h2" style={{marginBottom:"0"}}>Profil inställningar</h2>
                    <p className="user-settings-p">Uppdatera dina uppgifter, ändra lösenord och hantera leveransaddress.</p>
                </button>
                    </Link>
                </div>
                
                    <div className="user-footer-div">
                     <footer id="user-footer">
                    <p className="user-footer-p">© 2025 Foody Alla rättigheter förbehållna</p>
                </footer>
                </div>

                </div>

                

        </div>
    )
}