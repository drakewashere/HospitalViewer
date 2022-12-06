import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Home } from "./components/Home";
import { Hospitals } from './components/Hospitals';

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/hospitals',
    element: <Hospitals />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
