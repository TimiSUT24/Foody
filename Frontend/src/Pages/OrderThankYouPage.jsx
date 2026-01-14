import { Link } from "react-router-dom"
import { useAuth } from "../Context/AuthContext";
import { BsCheck2Circle } from "react-icons/bs";
import { PiPackage } from "react-icons/pi";
import { FaLongArrowAltRight } from "react-icons/fa";
import "../CSS/ThankYouPage.css"

export default function OrderThankYouPage(){

    const {user} = useAuth();
    
    return (
    <div className="thank-you-page">
      {/* Thank You Content */}
      <main className="thank-you-main">
        <div className="thank-you-card">
          <div className="thank-you-content">
            {/* Success Icon */}
            <div className="success-icon-wrapper">
              <div className="success-icon-circle">
                <BsCheck2Circle className="success-icon" />
              </div>
            </div>

            {/* Heading */}
            <h1 className="thank-you-heading">
              Tack för din order!
            </h1>
            
            {/* Customer Name */}
            <p className="customer-greeting">
              Hej <span className="customer-name">{user.username}</span>
            </p>

            {/* Order Info */}
            <div className="order-info-box">
              <div className="order-info-content">
                <PiPackage className="order-info-icon" />
                <span className="order-info-text">
                  Snart börjar vi packa din order. Orderbekräftelse har skickats via mail.
                </span>
              </div>
            </div>

            {/* CTA Button */}
            <Link to={"/user-page"}>
                <button className="cta-button">
                Gå till mina sidor
                <FaLongArrowAltRight className="arrow-icon" />
                </button>
            </Link>

            {/* Secondary Link */}
            <a href="/" className="secondary-link">
              Fortsätt handla
            </a>
          </div>
        </div>
      </main>
    </div>
  );
}