import { useLocation } from "react-router-dom"
import { useState, useEffect } from "react"
import { OrderService } from "../../Services/OrderService";
import OrderCard from "../../Components/OrderCard";
import ProfileSettings from "../../Components/ProfileSettings";
import OrderDetails from "../../Components/OrderDetails";
import "../../CSS/User/UserProfilePage.css"


export default function UserProfilePage(){
    const location = useLocation();
    const [activeTab, setActiveTab] = useState("orders");
    const [order, setOrder] = useState([]);
    const [selectedOrder, setSelectedOrder] = useState(null);

    useEffect(() => {
        OrderService.myOrders().then(setOrder)
    }, [])


    //Tabs
    useEffect (() => {
        if(location.state?.tab){
            setActiveTab(location.state.tab);
        }
    }, [location.state])

    return(
        <div className="user-profile">

            <div className="tabs">
                <button className="orders-btn" onClick={() => setActiveTab("orders")}>Mina beställningar</button>
                <button className="settings-btn" onClick={() => setActiveTab("settings")}>Inställningar</button>
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
                {!selectedOrder && activeTab === "settings" &&
                <ProfileSettings />}
            
            </div>
            
        </div>
    )
}