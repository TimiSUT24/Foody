import "../CSS/User/OrderDetails.css"

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
            <button className="back-btn" onClick={onBack}>⬅ Tillbaka till beställningar</button>

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
                        <img  id="shipping-img" src="/IMG/icons8-delivery-time-50.png" alt=""/>
                    </div>               
                <p>Väntar</p>
                </div>

                <div id="shipping-container">
                <div className="shipping-icon">
                        <img id="shipping-img" src="/IMG/icons8-box-64.png" alt=""/>
                    </div>   
                <p>Beställning</p>
                </div>

                <div id="shipping-container">
                <div className="shipping-icon">
                        <img  id="shipping-img" src="/IMG/icons8-delivery-50.png" alt=""/>
                    </div>   
                <p>Skickas</p>
                </div>

                <div id="shipping-container">
                <div className="shipping-icon">
                        <img  id="shipping-img" src="/IMG/icons8-delivered-48.png" alt=""/>
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
                            <img src="/IMG/icons8-box-64.png" id="details-img"/>
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
                          <img id="details-extra-image" src="/IMG/icons8-location-50.png" alt=""/>
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
                          <img id="details-extra-image" src="/IMG/icons8-magnetic-card-50.png" alt="" />
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