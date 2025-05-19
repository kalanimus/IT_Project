import React from "react";
import classes from './header.module.css';
import { Link } from "react-router-dom";
import Cookies from "js-cookie";
import userIcon from "../../assets/icons/userIcon.svg"
import Bell from "../../assets/icons/Bell.svg"

const Header = (props) => {
    // const [user, setUser] = useState([])
    const cookie = Cookies.get("auth")

    return (
        <><header className={classes.header}>
            <div className={classes.left_header}>
                <h1 className={classes.crypto_text}>KALM</h1>
            </div>
            <div className={classes.right_header}>
                {cookie && (<>
                    {/* <img className={classes.image}src={logo} alt=""/> */}
                </>)}
                {!cookie && (<>
                    <Link to="/Main" className={classes.links}>Главная</Link>
                    <Link to="/Teachers" className={classes.links}>Преподаватели</Link>
                    <Link to="/Survey" className={classes.links}>Опросы</Link>
                    {/* <Link to="/Notions">
                        <button className={classes.profileBell}>
                            <img src={Bell} alt="Уведомления" className={classes.Bell}></img>
                        </button>
                    </Link> */}
                    <Link to="/Cabinet">
                        <button className={classes.profileIcon}>
                            <img src={userIcon} alt="Профиль" className={classes.iconImage} />
                        </button>
                    </Link>
                </>)}
            </div>
        </header> 
        <div className={classes.palka}></div></>
    );
}

// function Header() {
//     return (
//         <div className={styles.header}>
//             <div className={styles.labels_text}>
//                 <p className={styles.crypto_text}>KALM</p>
//                 <div className={styles.right_text}>
//                 { <UserIcon className={styles.user_icon} />}
//                 <img src = "../assets/icons/userIcon.svg"></img>
//                 </div>
//             </div>
//             <div className={styles.palka}></div> 
//         </div>
//         // <head class="header">
//         //     <img src=""></img>
//         //     <div>
//         //         <ul>
//         //             <li>
//         //                 <button></button>
//         //             </li>
//         //             <li>
//         //                 <button></button>
//         //             </li>
//         //             <li>
//         //                 <button></button>
//         //             </li>
//         //             <li>
//         //                 <button>
//         //                     <img src= ""></img>
//         //                 </button>
//         //             </li>
//         //         </ul>
//         //     </div>
//         // </head>
//     );
// }
export default Header;