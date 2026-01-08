import { Link } from "react-router-dom"
import { useAuth } from "../Context/AuthContext";
import "../CSS/ThankYouPage.css"

export default function OrderThankYouPage(){

    const {user} = useAuth();
    
    return(
        <div className="thank-you-page">
            <h1>Tack för din order</h1>
            <p>{user.username}</p>
            <p>Snart börjar vi packa din order. Orderbekräftelse mail har skickats klicka på knappen nedanför för att komma till mina sidor</p>
            <Link to={"/user-page"}>
            <button className="thank-btn">Mina sidor</button>
            </Link>
        </div>
    )
}