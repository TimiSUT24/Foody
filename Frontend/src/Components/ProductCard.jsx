import {useCart} from '../Context/CartContext'
import {Link} from 'react-router-dom'
import { CiCirclePlus } from "react-icons/ci";
import { CiCircleMinus } from "react-icons/ci";
import "../CSS/ProductCard.css"

export default function ProductCard({product}){
    const {addToCart, removeFromCart, getQty} = useCart();
    const quantity = getQty(product.id);
    return(
        <div className="product-card">   
                    <Link to={`/product/${product.id}`} className="product-link">         
                    <img src={product.imageUrl} alt={product.name} style={{width:200, height:200}}/>
                    </Link>  

                    <div className="product-text">
                    <Link to={`/product/${product.id}`} className="product-link">
                    <h4 style={{lineHeight:1}}>{product.name}</h4>
                    </Link>
                    <p>{product.weightText} {product.comparePrice}</p>
                              
                    </div>
                    {product.hasOffer ? (
                        <div className="product-price">
                            <span style={{ textDecoration: "line-through", opacity: 0.6 }}>
                                {product.price} {product.currency}
                            </span>
                            <span style={{ color: "red", marginLeft: 8, fontWeight: 600 }}>
                                {product.finalPrice} {product.currency} {product.offerName}
                            </span>
                        </div>
                    ) : (
                        <p className="product-price">{product.price} {product.currency}</p>
                    )}
                    
                    {quantity === 0 ? (
                        <button className="product-add" onClick={() => addToCart(product)}>Lägg till</button>
                        ) : (
                            <div style={{display:"flex",gap:10,justifyContent:"center",height:47}}>
                        <button className="product-minus" onClick={() => removeFromCart(product.id)} style={{width:100}}><CiCircleMinus style={{width:22,height:22}}/></button>
                        <p>{quantity} st</p>
                        <button className="product-add" onClick={() => addToCart(product)} style={{width:100}}><CiCirclePlus style={{width:22,height:22}}/></button>
                    </div>
                    )}                     
                </div>  
    )
}