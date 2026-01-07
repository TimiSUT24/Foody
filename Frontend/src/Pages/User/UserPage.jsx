import { OrderService } from "../../Services/OrderService"
import { Link } from "react-router-dom";
import "../../CSS/User/UserPage.css";


export default function UserPage(){
    return (
        <div className="user-page">

            <div id="user-header">
                <h1 className="user-header-h1">Premiumlivsmedel, levererade färska</h1>
                <p className="user-header-p">Upptäck utsökta kvalitetsprodukter och den allra färskaste maten - direkt från noggrant utvalda kvalitetsleverantörer.</p>

            </div>

            <div id="user-content">

                <Link to={"/user-profile-page"}
                 state={{tab: "orders"}}
                 style={{textDecoration:"none"}}>
                <button id="user-order">
                    <div className="order-image">
                       <img src="/IMG/icons8-shopping-bag-50.png" style={{width:50,height:50}} />
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
                       <img src="/IMG/icons8-profile-settings-48.png" style={{width:50,height:50}} />
                    </div>
                    <h2 className="user-settings-h2" style={{marginBottom:"0"}}>Profil inställningar</h2>
                    <p className="user-settings-p">Uppdatera dina uppgifter, ändra lösenord och hantera leveransaddress.</p>
                </button>
                    </Link>

                </div>

                <footer id="user-footer">
                    <p className="user-footer-p">© 2025 Foody Alla rättigheter förbehållna</p>
                </footer>

        </div>
    )
}