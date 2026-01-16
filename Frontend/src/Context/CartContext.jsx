import {createContext,useContext,useState,useEffect} from 'react'

const CartContext= createContext();

export function CartProvider({children}){
    const [cart, setCart] = useState(() => {
        const saved = localStorage.getItem("cart")
        return saved ? JSON.parse(saved)

    :[]});

      useEffect(() => {
        localStorage.setItem("cart", JSON.stringify(cart));
    },[cart])

    //Add to cart
    const addToCart = (product) => {

           if(!product || typeof product !== "object"){
        console.error("product must be an object",product);
        return;
    }
        setCart(prev => {
            const existing = prev.find(p => p.id === product.id);
            if(existing){
                return prev.map(p => 
                    p.id === product.id ? {...p, qty: p.qty + 1} : p
                );
            }
            return [...prev, {...product, qty: 1}]
        });
    };

    //remove from cart
    const removeFromCart = (id) => {
        setCart(prev => {
            const item = prev.find(p => p.id === id);
            if(!item) return prev;

            if(item.qty > 1)
                return prev.map(p =>
            p.id === id ? {...p, qty: p.qty - 1}: p );

            return prev.filter(p => p.id !== id);
        })
    }

    const removeWholeProductFromCart = (product) => {
        setCart(prev => {
            const item = prev.find(p => p.id === product.id);
            if(!item) return prev;
            return prev.filter(p => p.id !== product.id);
        })
    }

    const getQty = (id) => {
       return cart.find(p => p.id === id)?.qty ?? 0;
    }

    //clear the whole cart
    const clearCart = () => {
        setCart([])
        localStorage.removeItem("cart");
    }

    const totalItems = cart.reduce((sum,item) => sum + item.qty, 0);
    const totalPrice = cart.reduce((sum,item) => sum + item.price * item.qty, 0);

    return (
        <CartContext.Provider value ={{
            cart,
            addToCart,
            removeFromCart,
            removeWholeProductFromCart,
            clearCart,
            getQty,
            totalItems,
            totalPrice
        }}>
            {children}
            </CartContext.Provider>
    );
}

export function useCart(){
    return useContext(CartContext);
}