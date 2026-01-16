import "../CSS/User/OrderCard.css"
import { PiPackage } from "react-icons/pi";


export default function OrderCard({order,onClick}){
    const statusMap = {
    Pending: { label: "Väntar", color: " rgb(255, 193, 7)", backgroundColor:"rgba(255, 193, 7, 0.1)", borderColor:"border-color: rgba(255, 193, 7, 0.3)"},
    Processing: { label: "Behandlas", color: "rgb(13, 202, 240)", backgroundColor:"rgba(13, 202, 240, 0.1)", borderColor:"rgba(13, 202, 240, 0.3)" },
    Shipped: { label: "Skickad", color: "rgb(13, 110, 253)", backgroundColor:"rgba(13, 110, 253, 0.1)", borderColor:"rgba(13, 110, 253, 0.3)" },
    Delivered: { label: "Levererad", color: "rgb(25, 135, 84)", backgroundColor:"rgba(25, 135, 84, 0.1)",borderColor:"rgba(25, 135, 84, 0.3)"},
  };

  const status = statusMap[order.orderStatus] || statusMap.Pending;

  const previewItems = order.orderItems.map(i => i.foodName).slice(0, 2).join(", ");

  const date = new Date(order.orderDate).toLocaleDateString("sv-SE", {
    year: "numeric",
    month: "long",
    day: "numeric"
  });

  return (
            <>
        <div className="order-card" onClick={onClick}>        
            <div className="order-container">               
                <div id="order-img-container">
                    <icon id="shopping-img"><PiPackage style={{width:30,height:30,color:"hsl(28, 80%, 40%)"}}/></icon>
                </div>
                
                <div className="order-num">
                    <div className="order-first">
                    <p className="" style={{fontWeight:"bold"}}>Order #{order.id.slice(0, 8)}</p>
                    <p className="">{date}</p>
                    
                    </div>

                    <div className="order-second">
                        <span style={{padding: "4px 10px",
                        borderRadius: "6px",
                        fontWeight: "600",
                        color: status.color,
                        backgroundColor: status.backgroundColor,
                        borderColor:status.borderColor,
                        border:"2px solid"}}>                    
                        {status.label}
                           </span>
                        
                    <p style={{color:"black",fontWeight:"bold"}}>{order.totalPrice.toFixed(2)} kr</p>
                    </div>
                </div>     
                
         
            </div>

            <div style={{width:"98%",height:1,backgroundColor:"black",placeSelf:"center"}}>
                </div>       
    
                
        <div className="order-container-outer">
          <p className="order-container-outer-p">
            {order.orderItems.length} produkter • {previewItems}
            {order.orderItems.length > 2 && " …"}
          </p>
        </div>

        </div>
        </>
  );
}