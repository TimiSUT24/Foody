import "../CSS/User/OrderDetails.css"
import { PiPackage } from "react-icons/pi";
import { MdOutlineLocalShipping } from "react-icons/md";
import { RxCounterClockwiseClock } from "react-icons/rx";
import { IoIosCheckmarkCircleOutline } from "react-icons/io";
import { HiOutlineLocationMarker } from "react-icons/hi";
import { CiCreditCard1 } from "react-icons/ci";
import { MdArrowBack } from "react-icons/md";


export default function OrderDetails({ order, onBack }) {
    const statusMap = {
        Pending: {
            label: "Väntar",
            color: "rgb(255, 193, 7)",
            backgroundColor: "rgba(255, 193, 7, 0.1)",
            borderColor: "rgba(255, 193, 7, 0.3)"
        },
        Processing: {
            label: "Behandlas",
            color: "rgb(13, 202, 240)",
            backgroundColor: "rgba(13, 202, 240, 0.1)",
            borderColor: "rgba(13, 202, 240, 0.3)"
        },
        Shipped: {
            label: "Skickad",
            color: "rgb(13, 110, 253)",
            backgroundColor: "rgba(13, 110, 253, 0.1)",
            borderColor: "rgba(13, 110, 253, 0.3)"
        },
        Delivered: {
            label: "Levererad",
            color: "rgb(25, 135, 84)",
            backgroundColor: "rgba(25, 135, 84, 0.1)",
            borderColor: "rgba(25, 135, 84, 0.3)"
        }
    };

    const status = statusMap[order.orderStatus];
    const date = new Date(order.orderDate).toLocaleDateString("sv-SE", {
        year: "numeric",
        month: "long",
        day: "numeric"
    });

    return (
        <div className="order-details">

            {/* Back button */}
            <button className="back-btn" onClick={onBack}><icon><MdArrowBack/></icon><p>Tillbaka till beställningar</p></button>

            <div className="order-details-content">
                {/* Header */}
            <div className="header-details">
                <div className="header-text">
                    <h2>Order #{order.id}</h2>
                    <p className="details-date">{date}</p>
                </div>
                 <div
                    className="details-status"
                    style={{
                        color: status.color,
                        backgroundColor: status.backgroundColor,
                        border: `2px solid ${status.borderColor}`
                    }}
                    >
                    {status.label}
                </div>
            </div>

            <div className="shipping-icons">
                <div id="shipping-container">
                    <div className="shipping-icon">
                        <icon id="shipping-img" style={{width:30,height:30}}><RxCounterClockwiseClock style={{width:30,height:30}}/></icon>
                    </div>               
                <p>Väntar</p>
                </div>

                <div id="shipping-container">
                <div className="shipping-icon">
                        <icon id="shipping-img" style={{width:30,height:30}}><PiPackage style={{width:30,height:30}}/></icon>
                    </div>   
                <p>Behandlas</p>
                </div>

                <div id="shipping-container">
                <div className="shipping-icon">
                        <icon id="shipping-img" style={{width:32,height:32}}><MdOutlineLocalShipping style={{width:30,height:30}}/></icon>
                    </div>   
                <p>Skickad</p>
                </div>

                <div id="shipping-container">
                <div className="shipping-icon">
                        <icon id="shipping-img" style={{width:30,height:30}}><IoIosCheckmarkCircleOutline style={{width:30,height:30}}/></icon>
                    </div>   
                <p>Levererad</p>
                </div>
                
            </div>
           <div id="br-div"></div>
            {/* Items */}
            <div className="details-items">
                <h3 style={{textAlign:"left"}}>Produkter</h3>
                {order.orderItems.map((item, index) => (
                    <div key={index} className="details-item">
                        <div style={{backgroundColor:"#efbe9bff",padding:5,marginRight:8,borderRadius:10}}>
                            <icon><PiPackage style={{width:50,height:50}}/></icon>
                        </div>
                        
                       <div style={{display:"flex", alignItems:"center",width:"920px",justifyContent:"space-between"}}>
                            <div>
                                <p className="item-name">{item.foodName}</p>
                                <p className="item-qty">Antal: {item.quantity}</p>
                            </div>
                                <div style={{width:"80px",textAlign:"center"}}>
                                    <p className="item-price" style={{fontWeight:"bold"}}>
                                    {(item.unitPrice * item.quantity).toFixed(2)} kr
                                </p>
                            </div>                      
                        </div>

                    </div>                 
                ))}
            </div>

              <div id="br-div"></div>

             <div className="details-prices">
                    
                    <div className="details-price">
                    <span >Delsumma:</span>
                    <span >{order.subTotal} kr</span>
                    </div>

                    
                    <div className="details-price">
                    <span>Frakt:</span>
                    <span>{order.shippingTax} kr</span>
                    </div>

                    <div className="details-price">
                    <span>Moms:</span>
                    <span>{order.moms} kr</span>
                    </div>
                </div>
                <div id="br-div"></div>

            {/* Total */}
            <div className="details-total">
                <p style={{fontWeight:"bold",fontSize:"20px"}}>Totalt:</p>
                <p style={{fontWeight:"bold",fontSize:"18px"}} className="total-price">{order.totalPrice.toFixed(2)} kr</p>
            </div>

            <div id="br-div"></div>

            <div className="details-extra">

                <div className="details-extra-content">
                   <div id="details-extra-img">
                          <icon id="details-extra-image"><HiOutlineLocationMarker style={{width:30,height:30}}/></icon>
                    </div>
                  
                    <div className="details-p">
                    <p style={{fontSize:"18px",color:"black"}}>Leveransadress</p>
                    <p>{order.shippingItems.adress}</p>
                    <p>{order.shippingItems.postalCode} {order.shippingItems.city}</p>
                    <p>{order.shippingItems.state}</p>
                    </div>   

                </div>

                <div className="details-extra-content">
                    <div id="details-extra-img">
                          <icon id="details-extra-image"><CiCreditCard1 style={{width:30,height:30}}/></icon>
                    </div>
                  
                    <div className="details-p">
                    <p style={{fontSize:"18px",color:"black"}}>Betalningsmetod</p>
                    <p>{order.paymentMethod}</p>
                    </div>                 

                </div>

            </div>

            </div>
            
        </div>
    );
}