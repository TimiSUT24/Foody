import {useCart} from "../Context/CartContext"
import api from "../Api/api"
import "../CSS/CartPage.css"

export default function CartPage(){
    const {cart, addToCart, removeFromCart, totalPrice} = useCart();

    const placeOrder = async () => {
        const body = {
            items: cart.map(item => ({
                foodId: item.id,
                quantity: item.qty
            }))
        }

        try{
            await api.post("/api/Order/create", body);
            alert("Order tillagd");
        }catch(err){
            console.error(err);
            alert("kunde inte lägga till order");
        }
    }

    return(
        <div className="cart-page">
            <h1 style={{display:"flex",gridArea:"h1"}}>Varukorg</h1>

            <div id="shipping-information" style={{display:"flex",flexDirection:"column",gap:"20px",padding:"20px"}}>
                <h2 style={{textAlign:"left"}}>LeveransInformation</h2>
                
                <div style={{display:"flex", justifyContent:"space-between"}}>
                    <div style={{display:"flex",flexDirection:"column",textAlign:"left",gap:"5px"}}>
                        <p style={{margin:"0"}}>Förnamn</p>
                        <input type="text" name="firstname" id="" placeholder="Jan" style={{width:"350px",paddingLeft:"10px"}} />
                    </div>
                    <div style={{display:"flex",flexDirection:"column",textAlign:"left",gap:"5px"}}>
                        <p style={{margin:"0"}}>Efternamn</p>
                        <input type="text" name="firstname" id="" placeholder="Jan" style={{width:"350px",paddingLeft:"10px"}} />
                    </div>              
                </div>

                <div style={{display:"flex", flexDirection:"column",gap:"20px"}}>
                    <div style={{display:"flex", flexDirection:"column", gap:"5px"}}>
                        <p style={{margin:"0",textAlign:"left"}}>E-post</p>
                        <input type="email" name="email" placeholder="jan@example.com" style={{paddingLeft:"10px"}} />
                    </div>
                    <div style={{display:"flex", flexDirection:"column",gap:"5px"}}>
                        <p style={{margin:"0",textAlign:"left"}}>Gatuadress</p>
                <input type="text" name="streetadress" placeholder="123 Malmgatan" style={{paddingLeft:"10px"}}/>
                    </div>              
                </div>

                <div style={{display:"flex", justifyContent:"space-between"}}>
                    <div style={{display:"flex",flexDirection:"column",textAlign:"left",gap:"5px"}}>
                        <p style={{margin:"0"}}>Ort</p>
                        <input type="text" name="city" placeholder="Stockholm" style={{width:"350px",paddingLeft:"10px"}}/>
                    </div>                 
                    <div style={{display:"flex",flexDirection:"column",textAlign:"left",gap:"5px"}}>
                        <p style={{margin:"0"}}>Postnummer</p>
                        <input className="postal-input" type="number" name="postalcode" placeholder="10011" style={{width:"350px",paddingLeft:"10px"}}/>
                    </div>                 
                </div>
                
            </div>

            <div id="cart-container">
            {cart.length === 0 ? (
                <p>Din varukorg är tom</p>
            ) : (
                <>
                <div className="cart-items">
                {cart.map(item => (
                    <div key={item.id} className="cart-item">                  
                        <img src={item.imageUrl} alt="" style={{width:50}}/>
                        <p className="p2" style={{fontSize:13}}>{item.name}</p>
                        <p style={{fontSize:13}}>{item.price} kr</p>

                        <div className="qty-controls">
                            <button onClick ={() => removeFromCart(item.id)} style={{borderRadius:10,borderStyle:"solid",borderWidth:1,backgroundColor:"lightGray",opacity:"60%"}}>-</button>
                            <span>{item.qty}</span>
                            <button onClick ={() => addToCart(item)} style={{borderRadius:10,borderStyle:"solid",borderWidth:1,backgroundColor:"lightGray",opacity:"60%"}}>+</button>                
                        </div>
                    </div>
                ))}
                </div>

                <div className="custom-hr"></div>

                <div className="checkout-prices">
                    <div className="checkout-price">
                    <span >Subtotal:</span>
                    <span >10 kr</span>
                    </div>

                    <div className="checkout-price">
                    <span >Frakt:</span>
                    <span >10 kr</span>
                    </div>

                    <div className="checkout-price">
                    <span >Moms:</span>
                    <span >10 kr</span>
                    </div>
                </div>

                <div className="custom-hr"></div>
                
                <h2>Totalt: {totalPrice.toFixed(2)} kr</h2>

                <button className="checkout-btn" onClick={placeOrder}>Lägg Order</button>
                </>
                
            )}
            </div>

            <div id="payment"></div>
        </div>
    )
}