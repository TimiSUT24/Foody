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
                <div>
                <img src="/IMG/icons8-shopping-bag-50.png" alt=""/>
                <p>Väntar</p>
                </div>

                <div>
                <img src="/IMG/icons8-shopping-bag-50.png" alt=""/>
                <p>Beställning</p>
                </div>

                <div>
                <img src="/IMG/icons8-shopping-bag-50.png" alt=""/>
                <p>Skickas</p>
                </div>

                <div>
                <img src="/IMG/icons8-shopping-bag-50.png" alt=""/>
                <p>Levererad</p>
                </div>
                
            </div>
           
            {/* Items */}
            <div className="details-items">
                <h3 style={{textAlign:"left"}}>Produkter</h3>
                {order.orderItems.map((item, index) => (
                    <div key={index} className="details-item">
                        <div style={{backgroundColor:"orange",padding:5,marginRight:8,borderRadius:10}}>
                            <img src="/IMG/icons8-shopping-bag-50.png" style={{width:40,height:40}} />
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

             <div className="details-prices">
                    
                    <div className="details-price">
                    <span >Subtotal:</span>
                    <span > kr</span>
                    </div>

                    
                    <div className="details-price">
                    <span>Frakt:</span>
                    <span>10 kr</span>
                    </div>

                    <div className="details-price">
                    <span>Moms:</span>
                    <span> kr</span>
                    </div>
                </div>

            {/* Total */}
            <div className="details-total">
                <p style={{fontWeight:"bold",fontSize:"20px"}}>Totalt:</p>
                <p style={{fontWeight:"bold"}} className="total-price">{order.totalPrice.toFixed(2)} kr</p>
            </div>

            <div className="details-extra">

                <div className="details-extra-content">
                   <div id="details-extra-img">
                          <img src="/IMG/icons8-shopping-bag-50.png" alt="" style={{width:40,height:40}} />
                    </div>
                  
                    <div className="details-p">
                    <p style={{fontSize:"18px"}}>Leveransadress</p>
                    <p>s</p>
                    <p>s</p>
                    <p>s</p>
                    </div>   

                </div>

                <div className="details-extra-content">
                    <div id="details-extra-img">
                          <img src="/IMG/icons8-shopping-bag-50.png" alt="" style={{width:40,height:40}} />
                    </div>
                  
                    <div className="details-p">
                    <p style={{fontSize:"18px"}}>Betalningsmetod</p>
                    <p>s</p>
                    <p>s</p>
                    <p>s</p>
                    </div>                 

                </div>

            </div>

            </div>
            
        </div>
    );
}