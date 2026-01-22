import "../CSS/AboutPage.css"
import { MdAccountCircle } from "react-icons/md";
import { FiShoppingBag } from "react-icons/fi";
import { LuNotepadText } from "react-icons/lu";
import { CiDeliveryTruck } from "react-icons/ci";
import { BsBack } from "react-icons/bs";

export default function AboutPage(){
    return <div className="about-page">       
                <header className="about-header">
                    <h1 className="about-header-h1">
                        Om oss
                    </h1>
                </header>

                <div className="about-description-div">
                    <h3>Välkommen till Foody</h3>
                    <p>Foody är din digitala matbutik där du enkelt kan beställa alla dina favoritvaror online. Vi erbjuder ett brett sortiment av livsmedel – från färska råvaror och mejeriprodukter till drycker och färdiglagad mat. Allt levereras bekvämt direkt till din dörr.</p>
                </div>

                <div className="about-usage-div">
                    <h3 className="about-usage-h3">Så fungerar det</h3>
                </div>

                <div className="about-usage-grid">
                    <div className="about-usage-grid-item">
                        <icon className="about-usage-icon-tag"><FiShoppingBag className="about-usage-icon"/></icon>
                        <h3>Brett utbud</h3>
                        <p>Handla allt från färska grönsaker till färdiga maträtter. Samma kvalitet som i din lokala ICA Kvantum.</p>
                    </div>

                    <div className="about-usage-grid-item">
                        <icon className="about-usage-icon-tag"><MdAccountCircle className="about-usage-icon"/></icon>
                        <h3>Skapa konto</h3>
                        <p>Registrera dig enkelt och få tillgång till personliga erbjudanden och smidigare beställningar.</p>
                    </div>

                    <div className="about-usage-grid-item">
                        <icon  className="about-usage-icon-tag"><LuNotepadText className="about-usage-icon"/></icon>
                        <h3>Orderhistorik</h3>
                        <p>Se alla dina tidigare beställningar och beställ enkelt samma varor igen med ett klick.</p>
                    </div>

                    <div className="about-usage-grid-item">
                        <icon className="about-usage-icon-tag"><CiDeliveryTruck className="about-usage-icon"/></icon>
                        <h3>Snabb leverans</h3>
                        <p>Vi levererar dina varor direkt till din dörr. Välj leveranstid som passar dig.</p>
                    </div>
                </div>

                <div className="about-account">
                    <h3>Ditt konto</h3>
                    <p>När du skapar ett konto hos Foody får du tillgång till:</p>
                    <ul>
                        <li>Komplett orderhistorik – se alla dina tidigare beställningar</li>
                        <li>Spara favoriter och inköpslistor</li>
                        <li>Snabbare utcheckning med sparade leveransadresser</li>
                        <li>Exklusiva erbjudanden och rabatter</li>
                    </ul>

                </div>
            </div>
}