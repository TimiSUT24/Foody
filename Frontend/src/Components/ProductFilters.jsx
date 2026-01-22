import {useState, useEffect, use} from 'react'
import {CategoryService} from '../Services/CategoryService'
import { ProductService } from '../Services/ProductService';
import { MdKeyboardArrowDown } from "react-icons/md";
import { useLocation } from 'react-router-dom';
import "../CSS/ProductFilter.css"

export default function ProductFilters({
    filters,
    updateFilter,
}){
    const [categories, setCategories] = useState([]);
    const [brands, setBrands] = useState([])
    const [menuOpen, setMenuOpen] = useState(false);
    const location = useLocation();

    const toggleMenu = () => setMenuOpen(p => !p)

     useEffect(() => {
      setMenuOpen(false);
    },[location.pathname])

    useEffect(() => {
    const loadCategories = async () => {
        try {
            const data = await CategoryService.getCategoryTree(); 
            setCategories(Array.isArray(data) ? data : data.categories);
        } catch (err) {
            console.error("Failed to load categories",err);
        }
    };

    loadCategories();
}, []);

    useEffect(() => {
        const loadBrands = async () => {
            try{
                const params = filters.categoryId ? {categoryId: filters.categoryId} : {};
                const data = await ProductService.getProductBrands(params);
                setBrands(data);
            }catch (err){
                console.error("Failed to load brands", err)
            }
        }
        loadBrands();
    }, [filters.categoryId])

    return (
        <div className={`filter-container ${menuOpen ? "open" : ""}`} >
            
            <div id="filter-container-header">
            <h3  id="filter-container-header-text" style={{display:"flex",alignItems:"center",gap:10}}> <img src="/IMG/icons8-sort-50.png" style={{height:40}}></img> Sortera:</h3>

            <div className="hamburger-filter" onClick={toggleMenu}>
                <span/>
                <span/>
                <span/>
            </div> 
            </div>

                <div className="search-div" style={{display:"flex", flexDirection:"row",position:"absolute",top:400,backgroundColor:"white",borderRadius:10,paddingLeft:15}}>
                    <img src="/IMG/icons8-search-48.png" alt="" style={{width:20,placeSelf:"center",paddingRight:5}} />              
                    <input 
                        type="text"
                        placeholder="Sök"
                        value={filters.name}                       
                        onChange={(e) => updateFilter({name: e.target.value})}
                        className="search">
                    </input>                  
                </div>    

                <select 
                    value ={filters.brand} 
                    onChange={(e) => updateFilter({brand: e.target.value})}
                    className="brand-select">
                        <option value="">Alla varumärken</option>
                        {brands.map((brand, index) => (
                            <option key={index} value={brand}>
                                {brand}
                            </option>
                        ))}
                </select>        

            <div className="category-menu">
                <button id="menu-button">Välj kategori <icon><MdKeyboardArrowDown style={{width:20,height:20,paddingLeft:52}}/></icon></button>
                

                <div className="menu-dropdown">
                    <div 
                        className="menu-item"
                        onClick={() => updateFilter({
                            categoryId: null,
                            subCategoryId: null,
                            subSubCategoryId: null
                        })}                       
                    >
                        Alla kategorier
                    </div>

                    {categories.map(cat => (
                        <div className="menu-item" key={cat.id}>
                            <span
                                onClick={() => 
                                        updateFilter({
                                                    categoryId: cat.id,
                                                    subCategoryId: null,
                                                    subSubCategoryId: null
                                                })
                                }
                            >
                                {cat.mainCategory}
                                {cat.subCategories?.length > 0 && <img src="/IMG/icons8-arrow-24.png" style={{width:15}}></img>}
                            </span>

                                {/*Sub category */}
                            <div className="submenu">
                                {cat.subCategories?.map(sc => (
                                    <div className="submenu-item" key={sc.id}>
                                        <span
                                            onClick={() =>                                              
                                                updateFilter({
                                                    categoryId: cat.id,
                                                    subCategoryId: sc.id,
                                                    subSubCategoryId: null
                                                })
                                            }
                                        >
                                            {sc.name}
                                            {sc.subSubCategories?.length > 0 && <img src="/IMG/icons8-arrow-24.png" style={{width:15}}></img>}
                                        </span>

                                        {/* Subsub category */}
                                        <div className="subsubmenu">
                                            {sc.subSubCategories?.map(ss => (
                                                <div 
                                                    key={ss.id} 
                                                    className="subsubmenu-item"
                                                    onClick={() => 
                                                        updateFilter({
                                                            categoryId: cat.id,
                                                            subCategoryId: sc.id,
                                                            subSubCategoryId: ss.id
                                                        })
                                                    }
                                                >
                                                    {ss.name}                                                    
                                                </div>
                                            ))}
                                        </div>

                                    </div>
                                ))}
                            </div>

                        </div>
                    ))}
                </div>
            </div>
                    
                    <div className="price-div">
                        <input  
                            type="number"
                            placeholder="Pris"
                            value={filters.price}
                            onChange={(e) =>{ 
                                const value = e.target.value
                                if(value === ""){
                                    updateFilter({price: ""})
                                    return;
                                }
                                updateFilter({price: Number(value)})}}
                            id="filter-price"
                            min="0"
                            style={{paddingLeft:"5px"}}
                        />
                    </div>
                   
                   <div className="offer-div">                        
                        <input type="checkbox" id="offer-checkbox" checked={filters.offer} onChange={(e) => updateFilter({offer: e.target.checked})}/>
                        <p style={{fontSize:14}}>Erbjudanden</p>
                   </div>
           

            <button id="reset-filter" onClick = {() =>                
                updateFilter({
                categoryId: null,
                subCategoryId: null,
                subSubCategoryId: null,
                price: "",
                brand: "",
                name: "",
                offer: false
            })}>Rensa filter</button>
            
        </div>
    )
}