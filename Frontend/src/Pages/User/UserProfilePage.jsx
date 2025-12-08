import { useLocation } from "react-router-dom"
import { useState, useEffect } from "react"
import { OrderService } from "../../Services/OrderService";
import OrderCard from "../../Components/OrderCard";
import "../../CSS/User/UserProfilePage.css"


export default function UserProfilePage(){
    const location = useLocation();
    const [activeTab, setActiveTab] = useState("orders")
    const [order, setOrder] = useState([])

    useEffect(() => {
        OrderService.myOrders().then(setOrder)
    }, [])


    //Tabs
    useEffect (() => {
        if(location.state?.tab === "orders"){
            setActiveTab("orders");
        }else{
            setActiveTab("settings")
        }
    }, [location.state])
console.log(activeTab)

    return(
        <div className="user-profile">

            <div className="tabs">
                <button className="orders-btn" onClick={() => setActiveTab("orders")}>Mina beställningar</button>
                <button className="settings-btn" onClick={() => setActiveTab("orders")}>Inställningar</button>
            </div>

            <div className="user-content">
                <div className="order-content">
                {activeTab === "orders" && order.map(order => (
                <OrderCard 
                key={order.id}
                order={order}
                />))}
                </div>
                {activeTab === "settings"}
                

            </div>
            
        </div>
    )
}