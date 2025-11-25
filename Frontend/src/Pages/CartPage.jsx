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
            <h1>Varukorg</h1>

            <div id="cart-container">
            {cart.length === 0 ? (
                <p>Din varukorg är tom</p>
            ) : (
                <>
                {cart.map(item => (
                    <div key={item.id} className="cart-item">
                   
                        <img src={item.imageUrl} alt="" style={{width:50}}/>
                        <p className="p2" style={{fontSize:13}}>{item.name}</p>
                        <p style={{fontSize:13}}>{item.price}</p>

                        <div className="qty-controls">
                            <button onClick ={() => removeFromCart(item.id)} style={{borderRadius:10,borderStyle:"solid",borderWidth:2}}>-</button>
                            <span>{item.qty}</span>
                            <button onClick ={() => addToCart(item)} style={{borderRadius:10,borderStyle:"solid",borderWidth:2}}>+</button>
                   
                        </div>
                    </div>
                ))}
                
                <h2>Total: {totalPrice.toFixed(2)} kr</h2>

                <button className="checkout-btn" onClick={placeOrder}>Lägg Order</button>
                </>
                
            )}
            </div>
        </div>
    )
}