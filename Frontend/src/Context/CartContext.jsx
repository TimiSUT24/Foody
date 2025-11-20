import {createContext,useContext,useState,useEffect} from 'react'

const CartContext= createContext();

export function CartProvider({children}){
    const [cart, setCart] = useState([]);

    const addToCart = (product) => {
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

    const totalItems = cart.reduce((sum,item) => sum = item.qty, 0);
    const totalPrice = cart.reduce((sum,item) => sum + item.price * item.qty, 0);

    return (
        <CartContext.Provider value ={{
            cart,
            addToCart,
            removeFromCart,
            totalItems,
            totalPrice
        }}>
            {children}
            </CartContext.Provider>
    );
}

export const useCart = () => useContext(CartContext);