SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Previous step: Create a Database named: `smallprojectapi`
--

CREATE TABLE `Ideas` (
  `id` varchar(8) NOT NULL,
  `idUsers` int(11) NOT NULL,
  `content` varchar(255) NOT NULL,
  `impact` int(11) NOT NULL,
  `ease` int(11) NOT NULL,
  `confidence` int(11) NOT NULL,
  `created_at` timestamp NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `Ideas` (`id`, `idUsers`, `content`, `impact`, `ease`, `confidence`, `created_at`) VALUES
('60ddb3a4', 1, 'the-content-user-1', 7, 8, 9, '2018-08-10 18:25:50'),
('c9182e61', 2, 'the-content-user-2', 8, 7, 5, '2018-08-10 18:51:41');

-- --------------------------------------------------------

CREATE TABLE `Users` (
  `id` int(11) NOT NULL,
  `email` varchar(100) NOT NULL,
  `name` varchar(100) NOT NULL,
  `password` varchar(255) NOT NULL,
  `refresh_token` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `Users` (`id`, `email`, `name`, `password`, `refresh_token`) VALUES
(1, 'jack-black@codementor.io', 'Jack Black', '1c86b1a2f041beb9c227ee7a27fcfa8c', '8moVf5I1g2Ykc8YCJ4QjGs5RUeB9/Xyi5rEqwIK3auM='),
(2, 'john@test.com', 'John Doe', '154bfe72ea6550c26e5f88ce693d01b7', NULL);

ALTER TABLE `Ideas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idUsers` (`idUsers`);

ALTER TABLE `Users`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `Users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
