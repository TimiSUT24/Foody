import {useCart} from '../Context/CartContext'
import {Link} from 'react-router-dom'
import "../CSS/ProductCard.css"

export default function ProductCard({product}){
    const {addToCart} = useCart();

    return(
        <div key={product.id} className="product-card">   
                    <Link to={`/product/${product.id}`} className="product-link">         
                    <img src={product.imageUrl} alt={product.name} style={{width:200, height:200}}/>
                    </Link>  

                    <div className="product-text">
                    <Link to={`/product/${product.id}`} className="product-link">
                    <h4 style={{lineHeight:1}}>{product.name}</h4>
                    </Link>
                    <p>{product.weightText} {product.comparePrice}</p>
                              
                    </div>
                    <p className="product-price">{product.price} {product.currency}</p>     
                    <button className="product-add" onClick={() => addToCart(product)}>Lägg till</button>
                </div>  
    )
}