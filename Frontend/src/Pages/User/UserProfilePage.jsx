import { useLocation,useNavigate } from "react-router-dom"
import { useState, useEffect } from "react"
import { OrderService } from "../../Services/OrderService";
import OrderCard from "../../Components/OrderCard";
import ProfileSettings from "../../Components/ProfileSettings";
import OrderDetails from "../../Components/OrderDetails";
import { useAuth } from "../../Context/AuthContext";
import { DiAptana } from "react-icons/di";
import { PiPackage } from "react-icons/pi";
import { IoLogOutOutline } from "react-icons/io5";
import "../../CSS/User/UserProfilePage.css"


export default function UserProfilePage(){
    const location = useLocation();
    const [activeTab, setActiveTab] = useState("orders");
    const [order, setOrder] = useState([]);
    const [selectedOrder, setSelectedOrder] = useState(null);
    const {logout} = useAuth();
    const navigate = useNavigate();
    const [status, setStatus] = useState(null)

    useEffect(() => {
        OrderService.myOrders(status).then(setOrder)
    }, [status])

    //Logout
    const handleLogout = async () => {
        await logout();
        navigate("/")
    } 

    //Tabs
    useEffect (() => {
        if(location.state?.tab){
            setActiveTab(location.state.tab);
        }
    }, [location.state])

    return(
        <div className="user-profile">

            <div className="tabs">      
                <button className="orders-btn" onClick={() => setActiveTab("orders")}><icon><PiPackage style={{width:23,height:23}}/></icon><p>Mina beställningar</p></button>                                         
                <div className="tab-logout">                  
                <button className="settings-btn" onClick={() => setActiveTab("settings")}><icon><DiAptana style={{width:23,height:23}}/></icon><p>Inställningar</p></button>                
                <button className="logout-btn" onClick={() => handleLogout()}><icon><IoLogOutOutline style={{width:23,height:23}}/></icon><p>Logga ut</p></button>
                </div>
            </div>

            <div className="user-content">

                

                {activeTab === "orders" && (
                    <>
                    {selectedOrder ? (
                    <OrderDetails
                    order={selectedOrder}
                    onBack={() => setSelectedOrder(null)}
                    />
                ): (
            <div className="order-content">
            <h2 style={{ textAlign: "left", margin: 0 }}>Orderhistorik</h2>
            <div className="order-filter-div">
                <div id="order-filter-button-div">
                    <button id="order-btn1" className="order-filter-button" onClick={() => setStatus("Pending")} style={{color:"rgb(255, 193, 7)",backgroundColor:"rgba(255, 193, 7, 0.1)",borderColor:"rgba(255, 193, 7, 0.3)"}}>Väntar</button>
                </div>
                <div id="order-filter-button-div">
                    <button id="order-btn2" className="order-filter-button" onClick={() => setStatus("Processing")} style={{color:"rgb(13, 202, 240)",backgroundColor:"rgba(13, 202, 240, 0.1)",borderColor:"rgba(13, 202, 240, 0.3)"}}>Behandlas</button>
                </div>
                <div id="order-filter-button-div">
                    <button id="order-btn3" className="order-filter-button" onClick={() => setStatus("Shipped")} style={{color:"rgb(13, 110, 253)",backgroundColor:"rgba(13, 110, 253, 0.1)",borderColor:"rgba(13, 110, 253, 0.3)"}}>Skickad</button>
                </div>
                <div id="order-filter-button-div">
                    <button id="order-btn4" className="order-filter-button" onClick={() => setStatus("Delivered")} style={{color:"rgb(25, 135, 84)",backgroundColor:"rgba(25, 135, 84, 0.1)",borderColor:"rgba(25, 135, 84, 0.3)"}}>Levererad</button>
                </div>
            </div>
            {order.map(order => (
                <OrderCard
                    key={order.id}
                    order={order}
                    onClick={() => {setSelectedOrder(order)}}
                />
            ))}
        </div>
                )}
        </>
    )}
                {activeTab === "settings" &&
                <ProfileSettings />}
            
            </div>
            
        </div>
    )
}