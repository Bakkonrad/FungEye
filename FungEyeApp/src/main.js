import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/js/bootstrap.bundle';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';


import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import './registerServiceWorker'

/* import the fontawesome core */
import { library } from '@fortawesome/fontawesome-svg-core'

/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

/* import specific icons */
import { faMagnifyingGlass, faTrash, faBan, faPen, faList, faArrowUp, faGear, faRightFromBracket, faLeftLong, faRightToBracket, faCircleUser, faPlus, faUndo, faAnglesRight, faThumbsUp, faThumbsDown, faComment, faFlag, faBookmark } from '@fortawesome/free-solid-svg-icons'

import { faBookmark as faBookmarkR} from '@fortawesome/free-regular-svg-icons'

/* add icons to the library */
library.add(faMagnifyingGlass, faTrash, faBan, faPen, faList, faArrowUp, faGear, faRightFromBracket, faLeftLong, faRightToBracket, faCircleUser, faPlus, faUndo, faAnglesRight, faThumbsUp, faThumbsDown, faComment, faFlag, faBookmark, faBookmarkR)


const app = createApp(App)

app.use(router)

app.component('font-awesome-icon', FontAwesomeIcon)

app.mount('#app')
