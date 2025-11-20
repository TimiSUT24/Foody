import {useCart} from '../Context/CartContext'
import "../CSS/HomePage.css"

export default function ProductCard({product}){
    const {addToCart} = useCart();

    return(
        <div key={product.id} className="product-card" style={{backgroundColor: "orange"}}>      
                    <img src={product.imageUrl} alt="" style={{width:200, height:200}}/>
                    <div className ="product-text">
                    <h4 style={{lineHeight:1}}>{product.name}</h4>
                    <p>{product.weightText} {product.comparePrice}</p>
                    <p>{product.price} {product.currency}</p>               
                    </div>
                    <button className="product-add" onClick={() => addToCart(product)}>Lägg till</button>
                </div>  
    )
}