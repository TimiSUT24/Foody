import {useCart} from "../Context/CartContext"
import api from "../Api/api"

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

            {cart.length === 0 ? (
                <p>Din varukorg är tom</p>
            ) : (
                <>
                {cart.map(item => (
                    <div key={item.id} className="cart-item">
                        <h3>{item.name}</h3>
                        <p>{item.price}</p>

                        <div className="qty-controls">
                            <button onClick ={() => removeFromCart(item.id)}>-</button>
                            <span>{item.qty}</span>
                            <button onClick ={() => addToCart(item)}>+</button>
                        </div>

                    </div>
                ))}
                
                <h2>Total: {totalPrice.toFixed(2)} kr</h2>

                <button className="checkout-btn" onClick={placeOrder}>Lägg Order</button>
                </>
            )}
        </div>
    )
}