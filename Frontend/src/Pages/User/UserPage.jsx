import { OrderService } from "../../Services/OrderService"
import "../../CSS/UserPage.css";




export default function UserPage(){
    return (
        <div className="user-page">

            <div id="user-header">
                <h1>Premiumlivsmedel, levererade färska</h1>
                <p>Upptäck utsökta kvalitetsprodukter och den allra färskaste maten - direkt från noggrant utvalda kvalitetsleverantörer.</p>

            </div>

            <div id="user-content">

                
                <button id="user-order">
                    <div className="order-image">
                       <img src="/IMG/icons8-shopping-bag-50.png" style={{width:50,height:50}} />
                    </div>
                    <h2 style={{marginBottom:"0"}}>Mina beställningar</h2>
                    <p>Se alla dina beställningar och följ leveranstatus i realtid.</p>
                 

                </button>

                <button id="user-settings">
                    <div className="order-image">
                       <img src="/IMG/icons8-profile-settings-48.png" style={{width:50,height:50}} />
                    </div>
                    <h2 style={{marginBottom:"0"}}>Profil inställningar</h2>
                    <p>Uppdatera dina uppgifter, ändra lösenord och hantera leveransaddress.</p>

                </button>


                </div>

                <footer>
                    <p>© 2025 Foody Alla rättigheter förbehållna</p>
                </footer>

        </div>
    )
}